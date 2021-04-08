import { inject, reset } from "aurelia-framework";
import { EventAggregator } from "aurelia-event-aggregator";

import { AssetCreated } from "./messages";
import { AssetService } from "../services/services";

import { Router } from "aurelia-router";

@inject(EventAggregator, AssetService, Router)
export class AssetCreate {
  private _assetService;
  private _ea;
  countries: any[];
  assetHold = {
    assetName: "",
    department: "",
    eMailAdressOfDepartment: "",
    countryOfDepartment: "",
    phone: "",
    broken: "",
  };
  router: Router;

  constructor(ea: EventAggregator, assetService: AssetService, router: Router) {
    this._assetService = assetService;
    this._ea = ea;
    this.router = router;
    this.countries = [];
  }

  getCountries() {
    this._assetService
      .getCountries()
      .then((data) => (this.countries = data))
      .catch((err) => console.log(err));
  }

  create() {
    let asset = JSON.parse(JSON.stringify(this.assetHold));
    if (asset.email === "") {
      return alert("You need to add information to your contact.");
    } else {
      this._assetService
        .createAsset(asset)
        .then((result) => {
          this._ea.publish(new AssetCreated(result));
        })
        .catch((err) => {
          let error = JSON.parse(err.response);
          alert(JSON.stringify(error.errors));
        });
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
