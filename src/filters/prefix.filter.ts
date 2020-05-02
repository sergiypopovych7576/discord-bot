import { Message, ClientEvents } from "discord.js";
import { BaseFilter } from "./base.filter";

import config from '../settings.json';

export class CommandPrefixFilter extends BaseFilter {
    public validate<K extends keyof ClientEvents>(...args: ClientEvents[K]): boolean {
        if (args[0] instanceof Message) {
            const message = args[0] as Message;

            return message.content.charAt(0) === config.commandPrefix;
        }

        return true;
    }
}