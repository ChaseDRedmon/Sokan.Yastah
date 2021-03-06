﻿import { Component } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";

import { Observable } from "rxjs";

import { FormOnResettingHandler, FormOnSavingHandler } from "../../common/form-component-base";

import { ICharacterGuildCreationModel, ICharacterGuildIdentityViewModel } from "./models";
import { GuildsService } from "./services";


@Component({
    selector: "guild-creation-page",
    templateUrl: "./guild-creation-page.ts.html",
    styleUrls: ["./guild-creation-page.ts.css"]
})
export class GuildCreationPage {

    public constructor(
            activatedRoute: ActivatedRoute,
            characterGuildsService: GuildsService,
            router: Router) {

        this._guildIdentities = characterGuildsService.observeIdentities();

        this._onResetting = () => Promise.resolve({
            name: "New Guild"
        });

        this._onSaving = async (model) => {
            let result = await characterGuildsService.create(model);

            if (result.guildId != null) {
                router.navigate([`../${result.guildId}`], { relativeTo: activatedRoute })
            }

            return result.error || null;
        };
    }

    public get guildIdentities(): Observable<ICharacterGuildIdentityViewModel[]> {
        return this._guildIdentities;
    }
    public get onResetting(): FormOnResettingHandler<ICharacterGuildCreationModel> {
        return this._onResetting;
    }
    public get onSaving(): FormOnSavingHandler<ICharacterGuildCreationModel> {
        return this._onSaving;
    }

    private readonly _guildIdentities: Observable<ICharacterGuildIdentityViewModel[]>;
    private readonly _onResetting: FormOnResettingHandler<ICharacterGuildCreationModel>;
    private readonly _onSaving: FormOnSavingHandler<ICharacterGuildCreationModel>;
}
