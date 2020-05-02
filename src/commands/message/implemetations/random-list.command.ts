import { BaseCommand } from "../../base.command";
import { ClientEvents, Message } from "discord.js";

import commandsConfiguration from '../../../message-commands.json';

export class RandomListCommand implements BaseCommand {
    public process<K extends keyof ClientEvents>(...args: ClientEvents[K]) {
        const msg = args[0] as Message;
        const params = commandsConfiguration.randomList.params;

        const items = msg.content.split(':')[1].split(' ');

        msg.channel.send(`${params.haveMessage}${items.length - 1}`);
        msg.channel.send(params.magicMessage);

        const randomResult = Math.floor(Math.random() * Math.floor(items.length));

        msg.channel.send(`:confetti_ball:`)
        msg.channel.send(items[randomResult]);
    }
}