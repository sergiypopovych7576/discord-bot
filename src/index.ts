import { BotClient } from "./client/bot-client";
import { CommandPrefixFilter } from "./filters/prefix.filter";
import { Logger } from "./logger/logger";

const client = new BotClient();

client.registerFilter(new CommandPrefixFilter());

client.subscribe('ready', async () => {
    Logger.log(`Logged in as: ${client.name}`);
});

client.subscribe('message', async (msg) => {
    Logger.log(`Resieved message: ${msg.content}`);
    msg.reply(`${msg.content} ? Гав гав!`);
})

client.login();