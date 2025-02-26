//
// Forked from https://github.com/JedWatson/react-input-autosize
//
import React from 'react';

type Props = {
    // className for the outer element
    className?: string;

    // default field value
    defaultValue?: any;

    // className for the input element
    inputClassName?: string;

    // css style for the input element
    inputStyle?: any;

    // minimum width for input element
    minWidth?: number | string;

    // onAutosize handler: function(newWidth) {}
    onAutosize?: (newWidth?: number | string) => void;

    // onChange handler: function(newValue) {}
    onChange: (newValue: any) => void;

    // placeholder text
    placeholder?: string;

    // don't collapse size to less than the placeholder
    placeholderIsMinWidth?: boolean;

    // css styles fro the outer element
    style?: any;

    // field value
    value: any;
    onKeyPress: (e: React.KeyboardEvent<HTMLInputElement>) => void;
};

type State = {
    inputWidth?: number | string;
};

class AutosizeInput extends React.Component<Props, State> {
    private _isMounted = false;
    private nodes: { input: HTMLInputElement | null; sizer: any; placeholderSizer: any } = {
        input: null,
        sizer: null,
        placeholderSizer: null
    };

    constructor(props: Props) {
        super(props);
        this.state = {
            inputWidth: this.props.minWidth
        };

        this.nodes = {
            input: null,
            sizer: null,
            placeholderSizer: null
        };
    }

    componentDidMount() {
        this._isMounted = true;
        this.copyInputStyles();
        this.updateInputWidth();
    }

    componentWillUnmount() {
        this._isMounted = false;
    }

    componentDidUpdate(prevProps: Props, prevState: State) {
        if (prevState.inputWidth !== this.state.inputWidth && typeof this.props.onAutosize === 'function') {
            this.props.onAutosize(this.state.inputWidth);
        }
        this.updateInputWidth();
    }
    copyInputStyles() {
        if (!this._isMounted || !window.getComputedStyle) {
            return;
        }

        const inputStyle = window.getComputedStyle(this.nodes.input as HTMLInputElement);
        if (!inputStyle) {
            return;
        }

        const widthNode = this.nodes.sizer;
        widthNode.style.fontSize = inputStyle.fontSize;
        widthNode.style.fontFamily = inputStyle.fontFamily;
        widthNode.style.fontWeight = inputStyle.fontWeight;
        widthNode.style.fontStyle = inputStyle.fontStyle;
        widthNode.style.letterSpacing = inputStyle.letterSpacing;

        if (this.props.placeholder) {
            const placeholderNode = this.nodes.placeholderSizer;
            placeholderNode.style.fontSize = inputStyle.fontSize;
            placeholderNode.style.fontFamily = inputStyle.fontFamily;
            placeholderNode.style.fontWeight = inputStyle.fontWeight;
            placeholderNode.style.fontStyle = inputStyle.fontStyle;
            placeholderNode.style.letterSpacing = inputStyle.letterSpacing;
        }
    }
    updateInputWidth() {
        if (!this._isMounted || typeof this.nodes.sizer.scrollWidth === 'undefined') {
            return;
        }
        let newInputWidth: number;
        if (this.props.placeholder && (!this.props.value || (this.props.value && this.props.placeholderIsMinWidth))) {
            newInputWidth = Math.max(this.nodes.sizer.scrollWidth, this.nodes.placeholderSizer.scrollWidth) + 2;
        } else {
            newInputWidth = this.nodes.sizer.scrollWidth + 2;
        }
        if (newInputWidth < (this.props.minWidth as number)) {
            newInputWidth = this.props.minWidth as number;
        }
        if (newInputWidth !== this.state.inputWidth) {
            this.setState({
                inputWidth: newInputWidth
            });
        }
    }
    getInput() {
        return this.nodes.input;
    }
    focus() {
        this.nodes.input?.focus();
    }
    blur() {
        this.nodes.input?.blur();
    }
    select() {
        this.nodes.input?.select();
    }
    render() {
        const sizerValue = [this.props.defaultValue, this.props.value, ''].reduce((previousValue, currentValue) => {
            if (previousValue !== null && previousValue !== undefined) {
                return previousValue;
            }
            return currentValue;
        });

        const wrapperStyle = this.props.style || {};
        if (!wrapperStyle.display) {
            wrapperStyle.display = 'inline-block';
        }

        const inputStyle = { ...this.props.inputStyle };
        inputStyle.width = this.state.inputWidth + 'px';
        inputStyle.boxSizing = 'content-box';

        const inputProps = { ...this.props };
        inputProps.className = this.props.inputClassName ?? '';
        inputProps.style = inputStyle;

        // ensure props meant for `AutosizeInput` don't end up on the `input`
        delete inputProps.inputClassName;
        delete inputProps.inputStyle;
        delete inputProps.minWidth;
        delete inputProps.placeholderIsMinWidth;

        return (
            <div className={this.props.className} style={wrapperStyle}>
                <input
                    {...inputProps}
                    ref={(node) => {
                        this.nodes.input = node;
                    }}
                />
                <div
                    ref={(node) => {
                        this.nodes.sizer = node;
                    }}
                    style={{
                        position: 'absolute',
                        top: 0,
                        left: 0,
                        visibility: 'hidden',
                        height: 0,
                        overflow: 'scroll',
                        whiteSpace: 'pre'
                    }}
                >
                    {sizerValue}
                </div>
                {this.props.placeholder && (
                    <div
                        ref={(node) => {
                            this.nodes.placeholderSizer = node;
                        }}
                        style={{
                            position: 'absolute',
                            top: 0,
                            left: 0,
                            visibility: 'hidden',
                            height: 0,
                            overflow: 'scroll',
                            whiteSpace: 'pre'
                        }}
                    >
                        {this.props.placeholder}
                    </div>
                )}
            </div>
        );
    }
}

export default AutosizeInput;
