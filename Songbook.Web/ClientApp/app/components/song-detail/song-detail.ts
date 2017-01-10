import { EventAggregator } from 'aurelia-event-aggregator';
import { HttpClient } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';
import { Messages } from '../app/app'

import Song = Songbook.Core.Song;

interface SongParams {
  id: number;
}

@inject(EventAggregator, HttpClient)
export class SongDetail {
  public song: Song;

  constructor(private ea: EventAggregator, private http: HttpClient) {
  }

  public activate(params: SongParams) {
    return this.http.fetch('/api/Songs/' + params.id)
      .then(result => result.json() as Promise<Song>)
      .then(data => {
        this.song = data;
        this.ea.publish(new Messages.SongViewed(data));
      });
  }

  public save() {
    this.song = this.song;
  }
}
