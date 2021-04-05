import { inject } from "aurelia-framework";
import { EventAggregator } from "aurelia-event-aggregator";

import { AssetService } from "../services/services";
import { AssetUpdated, AssetViewed } from "./messages";
import { areEqual } from "./utility";

import { Router } from "aurelia-router";

@inject(EventAggregator, AssetService, Router)
export class AssetDetail {
  private _assetService: AssetService;
  event: any;
  router: Router;
  routeConfig: any;
  activeAsset: any;
  originalAsset: {};

  constructor(eventAggregator, assetService: AssetService) {
    this.event = eventAggregator;
    this._assetService = assetService;
  }

  activate(params, routeConfig) {
    this.routeConfig = routeConfig;
    return this._assetService.getAsset(params.id).then((asset) => {
      this.activeAsset = asset;
      this.routeConfig.navModel.setTitle(this.activeAsset.firstName);
      this.originalAsset = asset;
      this.event.publish(new AssetViewed(this.activeAsset));
    });
  }

  get canSave() {
    return this.activeAsset.firstName && this.activeAsset.lastName;
  }

  save() {
    this._assetService
      .updateAsset(this.activeAsset.id, this.activeAsset)
      .then((asset) => {
        this.activeAsset = asset;
        this.routeConfig.navModel.setTitle(this.activeAsset.firstName);
        this.originalAsset = asset;
        this.event.publish(new AssetUpdated(this.activeAsset));
        window.history.back();
      });
  }

  canDeactivate() {
    if (!areEqual(this.originalAsset, this.activeAsset)) {
      let result = confirm(
        "You have unsaved changes. Are you sure you wish to leave?"
      );
      if (!result) {
        this.event.publish(new AssetViewed(this.activeAsset));
      }
      return result;
    }
    return true;
  }
}
