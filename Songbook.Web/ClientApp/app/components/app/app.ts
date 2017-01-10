import { Aurelia } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router';

export class App {
    router: Router;

    // TODO: auto-discover?
    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = (<any>window).Globals.hostingEnvironment;
        config.map([{
            route: [ '', 'home' ],
            name: 'home',
            settings: { icon: 'home' },
            moduleId: '../home/home',
            nav: true,
            title: 'Home'
        }, {
          route: 'songs',
          name: 'songs',
          settings: { icon: 'music' },
          moduleId: '../songs/songs',
          nav: true,
          title: 'Songs'
        }]);

        this.router = router;
    }
}
