import { h } from "preact";
import CodeAnalysis from './services/CodeAnalysis.service';

export default _ => (
    <div>
       <input type='button' value={"Get completions"} onClick={() => {
           CodeAnalysis.GetCompletionsAsync('asd', 1).then(x => console.log(x));
       }} />
    </div>
)