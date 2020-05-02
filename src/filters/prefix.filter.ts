import { Message, ClientEvents } from "discord.js";
import { BaseFilter } from "./base.filter";

import config from '../settings.json';

export class CommandPrefixFilter extends BaseFilter {
    public validate<K extends keyof ClientEvents>(...args: ClientEvents[K]): boolean {
        if (args[0] instanceof Message) {
            const message = args[0] as Message;

            const hasCommandPrefix = message.content.charAt(0) === config.commandPrefix;
            if(hasCommandPrefix) {
                this.cutCommandPrefix(message);
            }

            return hasCommandPrefix;
        }

        return true;
    }

    private cutCommandPrefix(msg: Message): void {
        msg.content = msg.content.substring(1);
    }
}