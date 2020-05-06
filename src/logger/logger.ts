import config from '../settings.json';

export class Logger {
    public static log(message: any) {
        console.log(`${config.logPrefix}${message}`);
    }
}
