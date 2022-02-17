
import React, { useEffect, useContext, useState } from 'react';
import { render, screen } from "@testing-library/react";
import '@testing-library/jest-dom';
//import shallow  from 'enzyme';
import MovieList from './../components/MovieList';


global.fetch = jest.fn(() =>
  Promise.resolve({
    json: () => Promise.resolve({ 
      'Movies' : [{
        'Title' :  'JestMovie'
      }], 
      'Errors' : []
    }),
  })
);

//need to mock context via enzyme

beforeEach(() => {
  //fetch.mockClear();
});

test("renders MovieList", async () => {
  render(<MovieList />);

  const name = screen.getByTestId("JestMovie");
  expect(name).toBeInTheDocument();
});