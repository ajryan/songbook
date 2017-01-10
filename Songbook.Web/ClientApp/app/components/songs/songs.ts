import { HttpClient } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';

@inject(HttpClient)
export class Songs {
    public songs: Songbook.Core.Song[];

    constructor(http: HttpClient) {
        http.fetch('/api/Songs/All')
            .then(result => result.json() as Promise<Songbook.Core.Song[]>)
            .then(data => {
                this.songs = data;
            });
    }
}
