import React, {useState } from 'react';
import { Route } from 'react-router';
import Home from './components/Home';
import MovieView from './components/MovieView';
import Layout from './Layout';

import { grommet, Grommet } from 'grommet';
import { MovieContext } from "./MovieContext.js";

import './custom.css';

const App = props => {
  const [context, setContext] = useState(null);

  return (
    <MovieContext.Provider value={[context, setContext]}>
      <Grommet theme={grommet} >
        <Layout >
            <Route exact path='/' component={Home} />
            <Route path='/view/:id' component={MovieView}  />
        </Layout>
      </Grommet>
    </MovieContext.Provider>
  );
};

export default App;




