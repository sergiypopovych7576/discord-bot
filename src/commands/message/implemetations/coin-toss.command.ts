import { BaseCommand } from '../../base.command';
import { ClientEvents, Message } from 'discord.js';

import commandsConfiguration from '../../../message-commands.json';

export class CoinTossCommand implements BaseCommand {
    public process<K extends keyof ClientEvents>(...args: ClientEvents[K]) {
        const msg = args[0] as Message;

        const params = commandsConfiguration.tossCoin.params;
        const randomResult = Math.floor(Math.random() * Math.floor(3));

        const result = randomResult === 1 ? params.positive : params.negative;
        msg.reply(result);
    }
}
