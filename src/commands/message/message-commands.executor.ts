
import { Message } from 'discord.js';
import { MessageCommandsFactory } from './message-commands.factory';

export class MessageCommandsExecutor {
    static process(msg: Message) {
       const command = MessageCommandsFactory.createCommand(msg);

       command.process(msg);
    }
}