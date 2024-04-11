import { useState } from 'react';

import styles from './App.module.scss';
import SearchBox from './components/SearchBox';
import { GlobalContext } from './context/global-context';

function App() {
    const [mobileView, setMobileView] = useState(false);
    const [username, setUsername] = useState<string | undefined>('');
    const [seachCounter, setSearchCounter] = useState(0);

    return (
        <div className={styles.container}>
            <GlobalContext.Provider value={{ mobileView, username, setUsername }}>
                <SearchBox />
            </GlobalContext.Provider>
        </div>
    );
}

export default App;
