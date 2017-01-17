import { EventAggregator } from 'aurelia-event-aggregator';
import { HttpClient, json } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';
import { Messages } from '../app/app'

import Song = Songbook.Core.Song;
import Lyric = Songbook.Core.Lyric;

interface SongParams {
  id: number;
}

@inject(EventAggregator, HttpClient)
export class SongDetail {
  public song: Song;

  constructor(private ea: EventAggregator, private http: HttpClient) {
  }

  public activate(params: SongParams) {
    return this.http.fetch(`/api/Songs/${params.id}`)
      .then(result => result.json() as Promise<Song>)
      .then(data => {
        this.song = data;
        this.ea.publish(new Messages.SongViewed(data));
      });
  }

  public addLyric() {
    this.song.Lyrics.push({
      Name: `Verse ${this.song.Lyrics.length + 1}`,
      Words: 'Add some words here'
    });
  }

  public moveLyricUp(lyric: Lyric) {
    var lyricIndex = this.song.Lyrics.indexOf(lyric);

    if (lyricIndex > 0) {
      var lyricBefore = this.song.Lyrics[lyricIndex - 1];
      this.song.Lyrics[lyricIndex - 1] = lyric;
      this.song.Lyrics[lyricIndex] = lyricBefore;
    }
  }

  public moveLyricDown(lyric: Lyric) {
    var lyricIndex = this.song.Lyrics.indexOf(lyric);

    if (lyricIndex < (this.song.Lyrics.length - 1)) {
      var lyricAfter = this.song.Lyrics[lyricIndex + 1];
      this.song.Lyrics[lyricIndex + 1] = lyric;
      this.song.Lyrics[lyricIndex] = lyricAfter;
    }
  }

  public deleteLyric(lyric: Lyric) {
    var lyricIndex = this.song.Lyrics.indexOf(lyric);
    this.song.Lyrics.splice(lyricIndex, 1);
  }

  public save() {
    this.createOrUpdate().then(() => {
      this.ea.publish(new Messages.SongUpdated(this.song));
    });
  }

  public delete() {
    var deletedId = this.song.Id;

    this.http.fetch(
      `/api/Songs/${deletedId}`,
      {
        method: 'delete'
      }).then(() => {
        this.ea.publish(new Messages.SongDeleted(deletedId));
    });
  }

  private createOrUpdate(): Promise<any> {
    var isCreate = (this.song.Id === null);

    if (isCreate) {
      return this.http.fetch(
        '/api/Songs', {
          method: 'post',
          body: json(this.song)
        }).then(result => result.json() as Promise<Song>)
        .then(data => this.song = data);
    } else {
      return this.http.fetch(
        `/api/Songs/${this.song.Id}`, {
          method: 'put',
          body: json(this.song)
        });
    }
  }
}
