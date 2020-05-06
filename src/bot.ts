import { BotClient } from './client/bot-client';
import { Logger } from './logger/logger';

import { CommandPrefixFilter } from './filters';
import { MessageCommandsExecutor } from './commands';

const client = new BotClient();

client.registerFilter(new CommandPrefixFilter());

client.subscribe('ready', async () => {
    Logger.log(`Logged in as: ${client.name}`);
});

client.subscribe('message', async (msg) => {
    Logger.log(`Resieved message: ${msg.content}`);

    MessageCommandsExecutor.process(msg);
    msg.reply(`Гав`);
});

client.login();
