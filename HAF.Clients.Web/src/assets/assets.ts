import {
  AssetViewed,
  AssetUpdated,
  AssetDeleted,
  AssetCreated,
} from "./messages";
import { AssetService } from "../services/services";

import { EventAggregator } from "aurelia-event-aggregator";
import { inject } from "aurelia-framework";

@inject(EventAggregator, AssetService)
export class AssetList {
  private _assetService: AssetService;
  ea: any;
  contacts: [] | any;
  selectedId: number;
  assets: any[];

  constructor(ea: EventAggregator, assetService: AssetService) {
    this.ea = ea;
    this.assets = [];
    this._assetService = assetService;

    ea.subscribe(AssetViewed, (msg) => this.select(msg.asset));
    ea.subscribe(AssetUpdated, (msg) => {
      let id = msg.asset.id;
      let found = this.assets.find((x) => x.id === id);
      Object.assign(found, msg.asset);
    });
    ea.subscribe(AssetDeleted, (msg) => {
      let deletedAsset = msg.asset;
      this.assets = this.assets.filter((asset) => asset !== deletedAsset);
    });
    ea.subscribe(AssetCreated, (msg) => {
      let asset = msg.asset;
      this.assets.push(asset);
    });
  }

  created() {
    this._assetService
      .getAssets()
      .then((data) => (this.assets = data))
      .catch((err) => console.log(err));
  }

  select(asset) {
    this.selectedId = asset.id;
    return true;
  }

  remove(asset) {
    if (confirm("Are you sure that you want to delete this contact?")) {
      this._assetService
        .deleteAsset(asset.id)
        .then((response) => {
          this.ea.publish(new AssetDeleted(asset));
        })
        .catch((err) => console.log(err));
    }
  }
}
