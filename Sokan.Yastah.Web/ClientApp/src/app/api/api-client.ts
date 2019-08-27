﻿import { Injectable } from "@angular/core";
import { Location } from "@angular/common";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class ApiClient {
    private static readonly _apiRoot: string
        = "/api";

    public constructor(
            httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    public delete<T>(url: string): Observable<T> {
        return this._httpClient.delete<T>(this.buildUrl(url));
    }

    public get<T>(url: string): Observable<T> {
        return this._httpClient.get<T>(this.buildUrl(url));
    }

    public post<T>(url: string, body: any | null): Observable<T> {
        return this._httpClient.post<T>(this.buildUrl(url), body);
    }

    public put<T>(url: string, body: any | null): Observable<T> {
        return this._httpClient.put<T>(this.buildUrl(url), body);
    }

    private buildUrl(url: string): string {
        return Location.joinWithSlash(ApiClient._apiRoot, url);
    }

    private readonly _httpClient: HttpClient;
}