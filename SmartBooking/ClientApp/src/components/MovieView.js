import React, { useEffect, useContext } from 'react';
import { useHistory } from 'react-router-dom';

import {
  Box,
  Text,
  Image,
  Button,
  Spinner
} from 'grommet';

import { defaultImagePath } from '../helpers'

import {
  Card,
  Heading,
  CardBody,
  Grid,
  Stack,
  DataTable
} from 'grommet';

import { MovieContext } from "./../MovieContext.js";

const MovieView = (props) => {
  const [movie, setMovie] = React.useState({});
  const [context, setContext] = useContext(MovieContext);
  const [showLoader, setShowLoader] = React.useState(true);
  const [allPrices, setAllPrices] = React.useState([]);
  const history = useHistory();

  if (!context) {
    history.replace({ pathname: `/` });
  }

  useEffect(() => {
    if (!context)
      return;

    async function GetMovieDetails() {
      const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(context)
      };

      fetch('/api/movies/details', requestOptions)
        .then(response => response.json())
        .then((movie) => {
          setMovie(movie.Movie);

          let allPrices = [];
          let providers = Object.keys(movie.Movie.AllPrices);

          providers.forEach(element => {
            allPrices.push({ 'provider': element, 'price': movie.Movie.AllPrices[element] });
          });

          allPrices.sort((a, b) => a.price - b.price);

          setAllPrices(allPrices)
          console.log({ movie });

          setShowLoader(false);
        });
    }

    GetMovieDetails();
  }, [context]);

  return (
    <Box>

      {showLoader ?

        (<Box className='load-spinner' >
          <Spinner size={"medium"} />
        </Box>)
        :

        <Box pad="small">

          <Heading level="4" >
            {movie.Title}
          </Heading>

          <Grid
            gap="medium"
            rows="medium"
            columns={{ count: 'fit', size: ['medium', 'small'] }}
          >

            <Box width={'medium'} height='medium'>

              <Image
                fit="cover"
                src={movie && movie.Posters && movie.Posters[1]}
                a11yTitle="movie title"
                alt={movie.Title}
                //fallback={(item.posters[0] == item.posters[1]) ? defaultImagePath : item.posters[1] }
                fallback={defaultImagePath}
              />


            </Box>

            <Card pad={'medium'} width="large" height={'medium'} key={movie.Title}>
              <Stack anchor="bottom-left">
                <CardBody>

                  <Box pad={"medium"}>
                    <Text weight={"bold"}>Cheapest Price </Text>
                    <Box pad={"medium"}>
                      <Text>{movie.EconomicPrice.Provider} <b> - {movie.EconomicPrice.Price} $</b></Text>
                    </Box>
                    <Button label="Buy"></Button>
                  </Box>

                  <Box pad={"medium"}>
                    <Text weight={"bolder"} pad={"medium"}>
                      All Prices
                    </Text>
                    <DataTable
                      columns={[
                        {
                          property: 'provider',
                          header: <Text>Provider</Text>,
                          primary: true,
                        },
                        {
                          property: 'price',
                          header: 'Price',
                          render: datum => (
                            <Text>{datum.price} $</Text>)
                        },
                      ]}
                      data={allPrices}
                    />
                  </Box>
                </CardBody>

              </Stack>
            </Card>

          </Grid>
        </Box>}
    </Box>
  );
};

export default MovieView;