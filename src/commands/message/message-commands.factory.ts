import { Message } from "discord.js";
import { BaseCommand } from "../base.command";
import commandsConfiguration from '../../message-commands.json';

import {
    CoinTossCommand,
    RandomListCommand
} from "./implemetations";

export class MessageCommandsFactory {
    public static createCommand(msg: Message): BaseCommand {
        if (msg.content === commandsConfiguration.tossCoin.command) {
            return new CoinTossCommand();
        }
        if (msg.content.includes(commandsConfiguration.randomList.command)) {
            return new RandomListCommand();
        }
    }
}