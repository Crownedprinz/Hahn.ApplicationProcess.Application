import { inject, reset } from "aurelia-framework";
import { EventAggregator } from "aurelia-event-aggregator";

import { AssetCreated } from "./messages";
import { AssetService } from "../services/services";

import { Router } from "aurelia-router";

@inject(EventAggregator, AssetService, Router)
export class AssetCreate {
  private _assetService;
  private _ea;
  contact = {
    firstName: "",
    lastName: "",
    email: "",
    phoneNumber: "",
  };
  router: Router;

  constructor(
    ea: EventAggregator,
    assetService: AssetService,
    router: Router
  ) {
    this._assetService = assetService;
    this._ea = ea;
    this.router = router;
  }

  create() {
    let asset = JSON.parse(JSON.stringify(this.asset));

    if (asset.email === "") {
      return alert("You need to add information to your contact.");
    } else {
      this._assetService
        .createContact(asset)
        .then((asset) => {
          this._ea.publish(new AssetCreated(asset));
        })
        .catch((err) => console.log(err));
      this.router.navigateToRoute("assets");
    }
  }
  asset(asset: any): string {
    throw new Error("Method not implemented.");
  }

  reset() {
    window.history.back();
  }
}
