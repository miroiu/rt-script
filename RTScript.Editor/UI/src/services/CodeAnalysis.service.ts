import DotNetService from './DotNet.service';
import Completion from '../models/Completion';

export default class CodeAnalysis {
    public static async GetCompletionsAsync(code: string, position: number): Promise<Completion[]> {
        try {
            const x = await DotNetService.invoke<Completion[]>('GetCompletionsAsync', code, position);
            console.log(x);
            return x;
        }
        catch (e) {
            console.log(e);
        }
    }
}
