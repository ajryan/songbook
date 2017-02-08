import { EventAggregator } from 'aurelia-event-aggregator';
import { HttpClient, json } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { Messages } from '../app/app';
import { SortStringValueConverter } from '../../valueconverters/sort-string';

import Song = Songbook.Core.Song;

@inject(EventAggregator, HttpClient, Router)
export class SongsList {
  public songs: Song[];
  public selectedId: string;
  public isRequesting: boolean;

  constructor(
      ea: EventAggregator,
      private http: HttpClient,
      private router: Router) {
    ea.subscribe(Messages.SongViewed, (msg: Messages.SongViewed) => this.select(msg.Song));

    ea.subscribe(Messages.SongUpdated,
      (msg: Messages.SongUpdated) => {
        var updatedSong = this.songs.filter(s => s.id === msg.Song.id)[0];
        Object.assign(updatedSong, msg.Song);
        this.songs.sort((a, b) => SortStringValueConverter.stringComparisonOrdinalIgnoreCase(a.Name, b.Name));
      });

    ea.subscribe(Messages.SongDeleted,
      (msg: Messages.SongDeleted) => {
        var deletedSong = this.songs.filter(s => s.id === msg.SongId)[0];
        var deletedIndex = this.songs.indexOf(deletedSong);
        this.songs.splice(deletedIndex, 1);

        if (this.selectedId === msg.SongId) {
          if (this.songs.length > 0) {
            if (deletedIndex >= this.songs.length)
              this.selectedId = this.songs[this.songs.length - 1].id;
            else
              this.selectedId = this.songs[deletedIndex].id;

            router.navigateToRoute('songs', { id: this.selectedId });
          } else {
            this.selectedId = null;
            router.navigateToRoute('no-selection');
          }
        }
      });

    this.refreshSongs();
  }

  public select(song: Song): boolean {
    this.selectedId = song.id;
    return true;
  }

  public addSong() {
    var newSong = {
      Id: null,
      Name: "New Song",
      Lyrics: []
    };

    this.isRequesting = true;
    this.http.fetch('/api/Songs',
      {
        method: 'post',
        body: json(newSong)
      })
      .then(result => result.json() as Promise<Song>)
      .then(song => {
        this.refreshSongs();
        this.router.navigateToRoute('songs', { id: song.id }, {});
      });
  }

  private refreshSongs() {
    this.isRequesting = true;
    this.http.fetch('/api/Songs')
      .then(result => result.json() as Promise<Song[]>)
      .then(data => {
        this.isRequesting = false;
        this.songs = data;
      });
  }
}
