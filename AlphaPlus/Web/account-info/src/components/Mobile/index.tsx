import { HashRouter, Route, Switch } from 'react-router-dom';

export default function AccountInfoMobile() {
    return (
        <HashRouter>
            <Switch>
                <Route exact path="/featureList"></Route>
            </Switch>
        </HashRouter>
    );
}
