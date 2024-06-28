import React from 'react';
import { HashRouter, Route, Switch } from 'react-router-dom';

import ContentItem from '../../models/content-item';
import Customer from '../../models/customer';
import ContentMobile from './ContentMobile';

type Props = {
    customerInfo?: Customer;
    redirectToBalanceInfoPage: boolean;
    accordionItems: ContentItem[];
};

type State = {
    redirectToBalanceInfoPage: boolean;
    redirectToFeatureList: boolean;
};

export default class AccountInfoMobile extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);

        this.state = {
            redirectToBalanceInfoPage: false,
            redirectToFeatureList: false
        };
    }

    componentWillReceiveProps(nextProps: Props) {
        const { customerInfo } = this.props;
        const nextCustomerInfo = nextProps.customerInfo;

        this.setState({
            redirectToFeatureList: nextCustomerInfo === undefined,
            redirectToBalanceInfoPage: nextProps.redirectToBalanceInfoPage
                ? nextCustomerInfo !== undefined && nextCustomerInfo !== customerInfo
                : false
        });
    }

    componentDidUpdate() {
        const { redirectToFeatureList, redirectToBalanceInfoPage } = this.state;
        if (redirectToFeatureList || redirectToBalanceInfoPage) {
            this.setState({
                redirectToFeatureList: false,
                redirectToBalanceInfoPage: false
            });
        }
    }

    render() {
        const { redirectToBalanceInfoPage, redirectToFeatureList } = this.state;
        const { accordionItems, customerInfo } = this.props;

        const routePropsArray: any[] = [
            {
                contentItems: accordionItems,
                pathname: '/featureList'
            },
            ...accordionItems,
            {
                ...accordionItems[1],
                contentItems: customerInfo !== undefined ? [] : accordionItems,
                pathname: ''
            }
        ];

        return (
            <HashRouter>
                <Switch>
                    {routePropsArray.map((routeProps, index) => {
                        return (
                            <Route
                                key={index}
                                exact
                                path={routeProps.pathname}
                                render={(props) => (
                                    <ContentMobile
                                        {...{
                                            path: props.location.pathname,
                                            redirectToBalanceInfoPage,
                                            redirectToFeatureList,
                                            ...routeProps
                                        }}
                                    />
                                )}
                            />
                        );
                    })}
                </Switch>
            </HashRouter>
        );
    }
}
