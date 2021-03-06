﻿import { HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Store } from "@ngrx/store";

import { from, of, throwError, Observable, Subject } from "rxjs";
import { catchError, filter, map, shareReplay, skip, switchMap, take, tap } from "rxjs/operators";

import { ApiClient } from "../../common/api-client";
import { isNotNullOrUndefined, IOperationError } from "../../common/types";
import { IAppState } from "../../state";

import {
    ICharacterGuildDivisionCreationModel,
    ICharacterGuildDivisionIdentityViewModel,
    ICharacterGuildCreationModel,
    ICharacterGuildIdentityViewModel,    ICharacterGuildDivisionUpdateModel,
    ICharacterGuildUpdateModel
} from "./models";
import {
    GuildDivisionsActionFactory,
    GuildsActionFactory,
} from "./state.actions";
import { GuildDivisionsSelectors, GuildsSelectors } from "./state.selectors";


export interface ICharacterGuildDivisionCreationResult {
    readonly divisionId?: number;
    readonly error?: IOperationError;
}
export interface ICharacterGuildCreationResult {
    readonly guildId?: number;
    readonly error?: IOperationError;
}


const guildsApiPath = "characters/guilds";

function divisionApiPath(
            guildId: number,
            divisionId: number):
        string {
    return `${divisionsApiPath(guildId)}/${divisionId}`;
}
function divisionsApiPath(
            guildId: number):
        string {
    return `${guildApiPath(guildId)}/divisions`;
}
function guildApiPath(
            guildId: number):
        string {
    return `${guildsApiPath}/${guildId}`;
}


@Injectable({
    providedIn: "root"
})
export class GuildDivisionsService {

    public constructor(
            apiClient: ApiClient,
            appState: Store<IAppState>) {

        this._apiClient = apiClient;
        this._appState = appState;

        this._onDeleted = new Subject();
    }

    public create(
                guildId: number,
                model: ICharacterGuildDivisionCreationModel):
            Promise<ICharacterGuildDivisionCreationResult> {
        return this._apiClient.post<number>(`${divisionsApiPath(guildId)}/new`, model)
            .pipe(
                tap(() => this._appState.dispatch(GuildDivisionsActionFactory.scheduleFetchIdentities({
                    guildId
                }))),
                map(divisionId => ({
                    guildId,
                    divisionId
                })),
                catchError((error: HttpErrorResponse) => (Math.trunc(error.status / 100) === 4)
                    ? of({
                        error: error.error
                    })
                    : throwError(error)))
            .toPromise();
    }

    public delete(
                guildId: number,
                divisionId: number):
            Promise<void> {
        return this._apiClient.delete<void>(divisionApiPath(guildId, divisionId))
            .pipe(
                catchError((error: HttpErrorResponse) => (error.status === 404)
                    ? of(null)
                    : throwError(error)),
                tap(() => this._appState.dispatch(GuildDivisionsActionFactory.scheduleFetchIdentities({
                    guildId
                }))),
                tap(() => this._onDeleted.next({
                    guildId,
                    divisionId,
                })),
                map(() => void 0))
            .toPromise();
    }

    public fetchIdentities(
                guildId: number):
            Promise<ICharacterGuildDivisionIdentityViewModel[] | null> {
        return this._appState.select(GuildDivisionsSelectors.identitiesIsFetching(guildId))
            .pipe(
                take(1),
                switchMap(isFetching => isFetching
                    ? this._appState.select(GuildDivisionsSelectors.identities(guildId))
                        .pipe(
                            skip(1),
                            take(1))
                    : of(null)
                        .pipe(
                            tap(() => this._appState.dispatch(GuildDivisionsActionFactory.beginFetchIdentities({
                                guildId
                            }))),
                            switchMap(() => this._apiClient.get<ICharacterGuildDivisionIdentityViewModel[]>(`${divisionsApiPath(guildId)}/identities`)),
                            catchError((error: HttpErrorResponse) => (error.status === 404)
                                ? of(null)
                                : throwError(error)),
                            tap(identities => this._appState.dispatch((identities == null)
                                ? GuildsActionFactory.remove({
                                    guildId
                                })
                                : GuildDivisionsActionFactory.storeIdentities({
                                    guildId,
                                    identities
                                }))))))
            .toPromise();
    }

    public fetchIdentity(
                guildId: number,
                divisionId: number):
            Promise<ICharacterGuildDivisionIdentityViewModel | null> {
        return this._appState.select(GuildDivisionsSelectors.identitiesIsFetching(guildId))
            .pipe(
                take(1),
                switchMap(isFetching => isFetching
                    ? this._appState.select(GuildDivisionsSelectors.identity(guildId, divisionId))
                        .pipe(
                            skip(1),
                            take(1))
                    : from(this.fetchIdentities(guildId))
                        .pipe(map(identities => identities && identities.find(identity => identity.id === divisionId) || null))))
            .toPromise();
    }

    public observeIdentities(
                guildId: number):
            Observable<ICharacterGuildDivisionIdentityViewModel[]> {
        return this._appState.select(GuildDivisionsSelectors.identitiesNeedsFetch(guildId))
            .pipe(
                tap(needsFetch => needsFetch
                    ? setTimeout(() => this.fetchIdentities(guildId))
                    : null),
                switchMap(() => this._appState.select(GuildDivisionsSelectors.identities(guildId))),
                filter(isNotNullOrUndefined),
                shareReplay());
    }

    public observeIdentity(
                guildId: number,
                divisionId: number):
            Observable<ICharacterGuildDivisionIdentityViewModel> {
        return this._appState.select(GuildDivisionsSelectors.identitiesNeedsFetch(guildId))
            .pipe(
                tap(needsFetch => needsFetch
                    ? setTimeout(() => this.fetchIdentities(guildId))
                    : null),
                switchMap(() => this._appState.select(GuildDivisionsSelectors.identity(guildId, divisionId))),
                filter(isNotNullOrUndefined),
                shareReplay());
    }

    public onDeleted(
                guildId: number,
                divisionId: number):
            Observable<void> {
        return this._onDeleted
            .pipe(
                filter(deleted => (deleted.guildId === guildId) && (deleted.divisionId === divisionId)),
                map(() => void 0));
    }

    public update(
                guildId: number,
                divisionId: number,
                model: ICharacterGuildDivisionUpdateModel):
            Promise<IOperationError | null> {
        return this._apiClient.put<void>(divisionApiPath(guildId, divisionId), model)
            .pipe(
                tap(() => this._appState.dispatch(GuildDivisionsActionFactory.scheduleFetchIdentities({
                    guildId: guildId
                }))),
                catchError((error: HttpErrorResponse) => (Math.trunc(error.status / 100) === 4)
                    ? of(error.error)
                    : throwError(error)))
            .toPromise();
    }

    private readonly _apiClient: ApiClient;
    private readonly _appState: Store<IAppState>;
    private readonly _onDeleted: Subject<{
        readonly guildId: number;
        readonly divisionId: number;
    }>;
}

@Injectable({
    providedIn: "root"
})
export class GuildsService {

    public constructor(
            apiClient: ApiClient,
            appState: Store<IAppState>) {

        this._apiClient = apiClient;
        this._appState = appState;

        this._onDeleted = new Subject();
    }

    public create(
                model: ICharacterGuildCreationModel):
            Promise<ICharacterGuildCreationResult> {
        return this._apiClient.post<number>(`${guildsApiPath}/new`, model)
            .pipe(
                tap(() => this._appState.dispatch(GuildsActionFactory.scheduleFetchIdentities())),
                map(guildId => ({
                    guildId
                })),
                catchError((error: HttpErrorResponse) => (Math.trunc(error.status / 100) === 4)
                    ? of({
                        error: error.error
                    })
                    : throwError(error)))
            .toPromise();
    }

    public delete(
                guildId: number):
            Promise<void> {
        return this._apiClient.delete<void>(guildApiPath(guildId))
            .pipe(
                catchError((error: HttpErrorResponse) => (error.status === 404)
                    ? of(null)
                    : throwError(error)),
                tap(() => this._appState.dispatch(GuildsActionFactory.scheduleFetchIdentities())),
                tap(() => this._appState.dispatch(GuildsActionFactory.remove({
                    guildId
                }))),
                tap(() => this._onDeleted.next(guildId)),
                map(() => void 0))
            .toPromise();
    }

    public fetchIdentities():
            Promise<ICharacterGuildIdentityViewModel[]> {
        return this._appState.select(GuildsSelectors.identitiesIsFetching)
            .pipe(
                take(1),
                switchMap(isFetching => isFetching
                    ? this._appState.select(GuildsSelectors.identities)
                        .pipe(
                            skip(1),
                            take(1))
                    : of(null)
                        .pipe(
                            tap(() => this._appState.dispatch(GuildsActionFactory.beginFetchIdentities())),
                            switchMap(() => this._apiClient.get<ICharacterGuildIdentityViewModel[]>(`${guildsApiPath}/identities`)),
                            tap(identities => this._appState.dispatch(GuildsActionFactory.storeIdentities({
                                identities
                            }))))),
                filter(isNotNullOrUndefined))
            .toPromise();
    }

    public fetchIdentity(
                guildId: number):
            Promise<ICharacterGuildIdentityViewModel | null> {
        return this._appState.select(GuildsSelectors.identitiesIsFetching)
            .pipe(
                take(1),
                switchMap(isFetching => isFetching
                    ? this._appState.select(GuildsSelectors.identity(guildId))
                        .pipe(
                            skip(1),
                            take(1))
                    : from(this.fetchIdentities())
                        .pipe(map(identities => identities.find(identity => identity.id === guildId) || null))))
            .toPromise();
    }

    public observeIdentities():
            Observable<ICharacterGuildIdentityViewModel[]> {
        return this._appState.select(GuildsSelectors.identitiesNeedsFetch)
            .pipe(
                tap(needsFetch => needsFetch
                    ? setTimeout(() => this.fetchIdentities())
                    : null),
                switchMap(() => this._appState.select(GuildsSelectors.identities)),
                filter(isNotNullOrUndefined),
                shareReplay());
    }

    public observeIdentity(
                guildId: number):
            Observable<ICharacterGuildIdentityViewModel> {
        return this._appState.select(GuildsSelectors.identitiesNeedsFetch)
            .pipe(
                tap(needsFetch => needsFetch
                    ? setTimeout(() => this.fetchIdentities())
                    : null),
                switchMap(() => this._appState.select(GuildsSelectors.identity(guildId))),
                filter(isNotNullOrUndefined),
                shareReplay());
    }

    public onDeleted(
                guildId: number):
            Observable<void> {
        return this._onDeleted
            .pipe(
                filter(deletedGuildId => deletedGuildId == guildId),
                map(() => void 0));
    }

    public update(
                guildId: number,
                model: ICharacterGuildUpdateModel):
            Promise<IOperationError | null> {
        return this._apiClient.put<void>(guildApiPath(guildId), model)
            .pipe(
                tap(() => this._appState.dispatch(GuildsActionFactory.scheduleFetchIdentities())),
                catchError((error: HttpErrorResponse) => (Math.trunc(error.status / 100) === 4)
                    ? of(error.error)
                    : throwError(error)))
            .toPromise();
    }

    private readonly _apiClient: ApiClient;
    private readonly _appState: Store<IAppState>;
    private readonly _onDeleted: Subject<number>;
}
