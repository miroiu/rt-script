declare var DotNet: any;
const _assemblyName = 'RTScript.Editor';

export default class DotNetService {
    public static invoke<T>(methodName: string, ...args: any[]): Promise<T> {
        return DotNet.invokeMethodAsync(_assemblyName, methodName, args);
    }
}
