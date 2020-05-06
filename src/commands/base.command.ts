import { ClientEvents } from 'discord.js';

export abstract class BaseCommand {
    public abstract process<K extends keyof ClientEvents>(...args: ClientEvents[K]);
}
