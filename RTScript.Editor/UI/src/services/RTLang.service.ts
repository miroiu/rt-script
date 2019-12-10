import DotNetService from './DotNet.service';
import Completion from '../models/Completion';
import Diagnostic from '../models/Diagnostic';

export default class RTLangService {
    public async GetCompletionsAsync(code: string, position: number): Promise<Completion[]> {
        try {
            return await DotNetService.invoke<Completion[]>('GetCompletionsAsync', code, position);
        }
        catch (e) {
            console.log(e);
            return [];
        }
    }

    public async GetDiagnosticsAsync(code: string): Promise<Diagnostic[]> {
        try {
            return await DotNetService.invoke<Diagnostic[]>('GetDiagnosticsAsync', code);
        }
        catch (e) {
            return [];
        }
    }
    
    public async ExecuteAsync(code: string): Promise<void> {
        try {
            await DotNetService.invoke<void>('ExecuteAsync', code);
        }
        catch (e) {
        }
    }
}
