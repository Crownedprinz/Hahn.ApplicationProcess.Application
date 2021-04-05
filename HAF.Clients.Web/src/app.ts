import { Aurelia, PLATFORM } from "aurelia-framework";
import { Router, RouterConfiguration } from "aurelia-router";

export class App {
  router: Router | undefined;

  configureRouter(config: RouterConfiguration, router: Router) {
    config.title = "Aurelia CRUD";
    config.map([
      {
        route: ["", "home"],
        name: "home",
        moduleId: PLATFORM.moduleName("./home/home"),
        title: "Home",
      },
      {
        route: "assets-list",
        name: "assets",
        moduleId: PLATFORM.moduleName("./assets/assets"),
        title: "Asset List",
      },
      {
        route: "assets-list/:id",
        name: "AssetDetail",
        moduleId: PLATFORM.moduleName("./assets/AssetDetail"),
      },
      {
        route: "assets-list/create",
        name: "AssetCreate",
        moduleId: PLATFORM.moduleName("./assets/AssetCreate"),
      },
    ]);

    this.router = router;
  }
}
