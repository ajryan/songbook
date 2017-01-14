import { Aurelia } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router';

export class App {
  router: Router;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = (<any>window).Globals.hostingEnvironment;
    config.map([
      { route: '',          moduleId: '../no-selection/no-selection', title: 'Select', name: 'no-selection' },
      { route: 'songs/:id', moduleId: '../song-detail/song-detail',   titls: 'Songs',  name: 'songs' }
    ]);

    this.router = router;
  }
}

export namespace Messages {
  export class SongUpdated {
    constructor(public Song: Songbook.Core.Song) {}
  }

  export class SongViewed {
    constructor(public Song: Songbook.Core.Song) {}
  }

  export class SongDeleted {
    constructor(public SongId: number) { }
  }
}
