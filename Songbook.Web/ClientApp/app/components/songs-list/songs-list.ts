import { EventAggregator } from 'aurelia-event-aggregator';
import { HttpClient } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';
import { Messages } from '../app/app'

import Song = Songbook.Core.Song;

@inject(EventAggregator, HttpClient)
export class SongsList {
  public songs: Song[];
  public selectedId: number;

  constructor(ea: EventAggregator, http: HttpClient) {
    ea.subscribe(Messages.SongViewed, msg => this.select(msg.Song));

    http.fetch('/api/Songs')
      .then(result => result.json() as Promise<Song[]>)
      .then(data => {
        this.songs = data;
      });
  }

  public select(song: Song): boolean {
    this.selectedId = song.Id;
    return true;
  }
}
