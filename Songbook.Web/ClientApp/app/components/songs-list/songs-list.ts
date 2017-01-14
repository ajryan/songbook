import { EventAggregator } from 'aurelia-event-aggregator';
import { HttpClient, json } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { Messages } from '../app/app'

import Song = Songbook.Core.Song;

@inject(EventAggregator, HttpClient, Router)
export class SongsList {
  public songs: Song[];
  public selectedId: number;
  public isRequesting: boolean;

  constructor(
      ea: EventAggregator,
      private http: HttpClient,
      private router: Router) {
    ea.subscribe(Messages.SongViewed, (msg: Messages.SongViewed) => this.select(msg.Song));

    ea.subscribe(Messages.SongUpdated,
      (msg: Messages.SongUpdated) => {
        var updatedSong = this.songs.filter(s => s.Id === msg.Song.Id)[0];
        Object.assign(updatedSong, msg.Song);
      });

    ea.subscribe(Messages.SongDeleted,
      (msg: Messages.SongDeleted) => {
        var deletedSong = this.songs.filter(s => s.Id === msg.SongId)[0];
        var deletedIndex = this.songs.indexOf(deletedSong);
        this.songs.splice(deletedIndex, 1);

        if (this.selectedId === msg.SongId) {
          if (deletedIndex >= this.songs.length)
            this.selectedId = this.songs[this.songs.length - 1].Id;
          else if (this.songs.length > 0)
            this.selectedId = this.songs[deletedIndex].Id;
          else
            router.navigateToRoute('no-selection');
        }
      });

    this.refreshSongs();
  }

  public select(song: Song): boolean {
    this.selectedId = song.Id;
    return true;
  }

  public addSong() {
    var newSong = {
      Id: 0,
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
        this.router.navigateToRoute('songs', { id: song.Id }, {});
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
