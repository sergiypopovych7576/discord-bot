import { Client, ClientEvents } from "discord.js"
import { BaseFilter } from "../filters/base.filter";

import config from '../settings.json';

export class BotClient {
    private _client = new Client();
    private _filters: BaseFilter[] = [];


    public get name(): string {
        return this._client.user.username;
    }

    public login(): void {
        this._client.login(config.apiKey);
    }

    public registerFilter(filter: BaseFilter) {
        this._filters.push(filter);
    }

    public subscribe<K extends keyof ClientEvents>(event: K, listener: (...args: ClientEvents[K]) => void) {
        this._client.on(event, (...args: ClientEvents[K]) => {
            const filterValidationResult = this.validateFilters(...args);

            if (filterValidationResult) {
                listener.call(this, ...args);
            }
        });
    }

    private validateFilters<K extends keyof ClientEvents>(...args: ClientEvents[K]): boolean {
        return this._filters.map(filter => filter.validate(...args))
            .filter(filterResult => !filterResult).length === 0;
    }
}

