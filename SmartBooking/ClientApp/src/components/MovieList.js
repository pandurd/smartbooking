import React, { useEffect, useContext } from 'react';

import { useHistory } from 'react-router-dom';

import { Box, Text, Grid } from 'grommet';
import { Spinner, Pagination } from 'grommet';

import { defaultImagePath } from './../helpers'

import {
  Card,
  Heading,
  CardBody,
  CardFooter,
  Image,
  Stack,
} from 'grommet';

import { MovieContext } from "./../MovieContext";

const MovieList = () => {
  const [availableMovies, setAvailableMovies] = React.useState([]);
  const [showError, setShowError] = React.useState(false);
  const [showLoader, setShowLoader] = React.useState(true);
  const [context, setContext] = useContext(MovieContext);
  const history = useHistory();

  async function fetchMovies() {
    setShowLoader(true);

    try {
      const res = await fetch(`/api/movies`);
      const jsonRes = await res.json();
      setAvailableMovies(jsonRes.Movies);
      setShowLoader(false);

      if (jsonRes.Errors.length > 0) {
        //can be modified to show only once service is down
        setShowError(true)
      }

    } catch {
      setShowError(true)
    }
  };

  useEffect(() => {
    fetchMovies();
  }, []);

  const showMovieView = (movie) => {
    setContext(movie);
    history.replace({ pathname: `/view/${movie.Title.toLowerCase().replaceAll(' ', '_')}` });
  }

  return (
    <Box style={{ height: '72vh', overflowY: 'scroll' }}>

      {showError && <Box>
        <Text> Sorry, Our services are down. Please try again after some time </Text>
      </Box>}

      {showLoader ?
        <Box className='load-spinner' >
          <Spinner size={"medium"} />
        </Box> :
        <Box pad="small">
          <Box>
            <Heading level="3" >
              Available Movies
            </Heading>
            <Grid
              gap="small"
              rows="small"
              columns={{ count: 'fit', size: 'small' }}
            >
              {availableMovies.map((item) => (
                <Card width="small" height="small" key={item.Title}
                  onClick={() => showMovieView(item)}
                  data-testid={item.Title} >
                  <Stack anchor="bottom-left">
                    <CardBody height="small">
                      <Image
                        fit="cover"
                        src={item.Posters[0]}
                        a11yTitle="movie title"
                        alt={item.Title}
                        //fallback={(item.posters[0] == item.posters[1]) ? defaultImagePath : item.posters[1] }
                        fallback={defaultImagePath}
                      />
                    </CardBody>
                    <CardFooter
                      pad={{ horizontal: 'small' }}
                      background="#000000A0"
                      justify="start"
                    >
                      <Box>
                        {item.Title}
                      </Box>
                    </CardFooter>
                  </Stack>
                </Card>
              ))}
            </Grid>
          </Box>
        </Box>}
    </Box>
  );
};

export default MovieList;
