import { Message } from "discord.js";
import { BaseCommand } from "../base.command";
import { CoinTossCommand } from "./implemetations";

import commandsConfiguration from '../../message-commands.json';

export class MessageCommandsFactory {
    public static createCommand(msg: Message): BaseCommand {
        if (msg.content === commandsConfiguration.tossCoin.command) {
            return new CoinTossCommand();
        }
    }
}