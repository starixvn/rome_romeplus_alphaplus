import React from 'react';
import { Redirect } from 'react-router-dom';

import ContentItem from '../../models/content-item';
import ContentPage from './ContentPage';
import FeatureList from './FeatureList';

interface Props extends ContentItem {
    path: string;
    redirectToBalanceInfoPage: boolean;
    redirectToFeatureList: boolean;
    contentItems?: ContentItem[];
}

export default class ContentMobile extends React.Component<Props> {
    private readonly featureListPath = '/featureList';
    private readonly balanceInfoPath = '/balanceInfo';

    constructor(props: Props) {
        super(props);
    }

    shouldComponentUpdate(props: Props) {
        const { redirectToBalanceInfoPage, redirectToFeatureList, path } = props;
        const balanceInfoRedirectBackToItself = path === this.balanceInfoPath && redirectToBalanceInfoPage;
        const featureListRedirectBackToItself = path === this.featureListPath && redirectToFeatureList;

        return !balanceInfoRedirectBackToItself && !featureListRedirectBackToItself;
    }

    render() {
        const {
            iconClass,
            featureText,
            children,
            fetchDataFromServer,
            contentItems = [],
            redirectToBalanceInfoPage = false,
            redirectToFeatureList = false
        } = this.props;

        if (redirectToBalanceInfoPage || redirectToFeatureList) {
            return (
                <Redirect
                    to={{
                        pathname: redirectToBalanceInfoPage ? this.balanceInfoPath : this.featureListPath
                    }}
                />
            );
        }

        return contentItems.length === 0 ? (
            <ContentPage {...{ iconClass, featureText, children, fetchDataFromServer }} />
        ) : (
            <FeatureList accordionItems={contentItems} />
        );
    }
}
