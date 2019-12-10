import { h, createRef, Component } from 'preact';
import CM from '../vendor/codemirror.js';
import '../vendor/codemirror.css';
import RTLangService from './services/RTLang.service.js';

export interface Props {
    config?: {
        lineNumbers: boolean,
        value: string,
        mode: 'javascript',
        theme: 'monokai'
    },
    service: RTLangService
}

export interface State {
    code: string
}

export default class Editor extends Component<Props, State> {
    ref = createRef();
    editor: any;

    componentDidMount() {
        const { config } = this.props;
        this.editor = CM.fromTextArea(this.ref.current, config);
    }

    render() {
        return (
            <textarea ref={this.ref} />
        );
    }
}
