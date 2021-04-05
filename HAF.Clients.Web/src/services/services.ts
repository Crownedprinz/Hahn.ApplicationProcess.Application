import { HttpClient } from "aurelia-http-client";
import { inject } from "aurelia-framework";

import api from "../config/api";

@inject(HttpClient)
export class AssetService {
  private http: HttpClient;
  assets = [];

  constructor(http: HttpClient) {
    http.configure((x) => x.withBaseUrl(api.dev + "/asset/"));
    this.http = http;
  }

  getAssets() {
    return new Promise<any>((resolve, reject)=>{
    this.assets = JSON.parse(
            '[{"firstName": "John", "id":1, "lastName": "Ademola", "email":"ademolajhon@gmail.com","phoneNumber":"08134734540"}]'
          );
          resolve(this.assets);
    });
    // let promise = new Promise<any>((resolve, reject) => {
    //   this.http
    //     .get('')
    //     .then((data) => {
    //       this.assets = JSON.parse(data.response);
    //       this.assets = JSON.parse(
    //         '[{"firstName": "John", "id":1, "lastName": "Ademola", "email":"ademolajhon@gmail.com","phoneNumber":"08134734540"}]'
    //       );
    //       resolve(this.assets);
    //     })
    //     .catch((err) => reject(err));
    // });
    // return promise;
  }

  createAsset(asset) {
    let promise = new Promise((resolve, reject) => {
      this.http
        .post("", asset)
        .then((data) => {
          let result = JSON.parse(data.response);
          resolve(result);
        })
        .catch((err) => reject(err));
    });
    return promise;
  }

  getAsset(id) {
    let promise = new Promise((resolve, reject) => {
      this.http
        .get(id)
        .then((response) => {
          let result = JSON.parse(response.response);
          result = this.assets = JSON.parse(
            '{"firstName": "John", "id":1, "lastName": "Ademola", "email":"ademolajhon@gmail.com","phoneNumber":"08134734540"}'
          );
    
          resolve(result);
        })
        .catch((err) => reject(err));
    });
    return promise;
  }

  deleteAsset(id) {
    let promise = new Promise((resolve, reject) => {
      this.http
        .delete(id)
        .then((response) => {
          let result = JSON.parse(response.response);
          resolve(result);
        })
        .catch((err) => reject(err));
    });
    return promise;
  }

  updateAsset(id, asset) {
    let promise = new Promise((resolve, reject) => {
      this.http
        .put(id, asset)
        .then((response) => {
          let result = JSON.parse(response.response);
          resolve(result);
        })
        .catch((err) => reject(err));
    });
    return promise;
  }
}
