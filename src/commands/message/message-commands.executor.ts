
import { Message } from 'discord.js';
import { MessageCommandsFactory } from './message-commands.factory';

export class MessageCommandsExecutor {
    public static process(msg: Message) {
       const command = MessageCommandsFactory.createCommand(msg);

       command.process(msg);
    }
}
