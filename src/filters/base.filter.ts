import { ClientEvents } from "discord.js";

export abstract class BaseFilter {
    public abstract validate<K extends keyof ClientEvents>(...args: ClientEvents[K]): boolean;
}