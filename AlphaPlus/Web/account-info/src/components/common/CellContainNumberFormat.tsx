import { NumericFormat } from 'react-number-format';

import CustomAnchorTag from './CustomAnchorTag';

const CellContainNumberFormat = ({ classNames = '', value = 0, hrefLink = '' }) => {
    let formatClassNames = 'number';

    if (value < 0) {
        formatClassNames = 'number-red';
    } else if (value > 0 && hrefLink !== null && hrefLink !== '') {
        formatClassNames = 'number-link';
    }

    return (
        <div className={`${classNames} ${formatClassNames}`}>
            <NumericFormat
                value={value}
                displayType={'text'}
                decimalScale={2}
                fixedDecimalScale={true}
                thousandSeparator={true}
                renderText={(displayedValue) =>
                    hrefLink === null || hrefLink === '' ? (
                        <span>{displayedValue}</span>
                    ) : (
                        <CustomAnchorTag {...{ href: hrefLink }}>{displayedValue}</CustomAnchorTag>
                    )
                }
            />
        </div>
    );
};

export default CellContainNumberFormat;
