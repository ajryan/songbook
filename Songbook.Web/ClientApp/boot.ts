import { Aurelia } from 'aurelia-framework';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap';
import * as $ from 'jquery';

// The value is supplied by Webpack during the build
// TODO: use ASPNETCORE_ENVIRONMENT instead, then no longer need Global injection (this could be an issue on the template repo too)
declare const IS_DEV_BUILD: boolean;

export function configure(aurelia: Aurelia) {
    aurelia.use.standardConfiguration();

    if (IS_DEV_BUILD) {
        aurelia.use.developmentLogging();
    }

    aurelia
        .start()
        .then(() => aurelia.setRoot('app/components/app/app'));
}
