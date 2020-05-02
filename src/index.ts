import { Client } from 'discord.js';
import config from './settings.json';
import { Logger } from './logger';

const client = new Client();

client.on('ready', async () => {
    Logger.log(`Logged in as: ${client.user?.username}`);
});

client.on('message', async (msg) => {
    if(!msg.author.bot) {
        Logger.log(`Resieved message: ${msg.content}`);
        msg.reply('Гаф');
    }
});

client.login(config.apiKey);