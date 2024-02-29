import styles from './App.module.scss';
import SearchBox from './components/SearchBox';

function App() {
    return (
        <div className={styles.container}>
            <SearchBox />
        </div>
    );
}

export default App;
