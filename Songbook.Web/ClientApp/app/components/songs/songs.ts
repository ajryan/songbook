// The following line is a workaround for aurelia-fetch-client requiring the type UrlSearchParams
// to exist in global scope, but that type not being declared in any public @types/* package.
/// <reference path="../../../../node_modules/aurelia-fetch-client/doc/url.d.ts" />

import { HttpClient } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';

@inject(HttpClient)
export class Fetchdata {
    public songs: Songbook.Core.Song[];

    constructor(http: HttpClient) {
        http.fetch('/api/Songs/All')
            .then(result => result.json() as Promise<Songbook.Core.Song[]>)
            .then(data => {
                this.songs = data;
            });
    }
}
