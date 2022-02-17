import React from 'react';
import { useHistory } from "react-router-dom";
import { 
    Anchor,
    Avatar,
    Button,
    Box,
    Nav,
    Main,
    Header,
    Sidebar } 
from 'grommet';
import * as Icons from 'grommet-icons';

const Layout = (props) => {
  const history = useHistory();

  return(
    <Box pad='none' direction="row" >
      <Sidebar background="brand" 
        style={{ minHeight: '100vh' }}
        header={
          <Avatar src="//s.gravatar.com/avatar/b7fb138d53ba0f573212ccce38a7c43b?s=80" />
        }
        footer={
          <Button icon={<Icons.Help />} hoverIndicator />
        }>
        <Nav gap="small" onClick={() => history.replace({ pathname: `/`})}>
          <Button icon={<Icons.Projects />} hoverIndicator />
        </Nav>
      </Sidebar>

      <Main>
       <Header background="light-4" pad="medium" height="xsmall">
        <Anchor
          href="/"
          icon={<Icons.Ticket color="brand" />}
          label="Smart Booking"
        />
       </Header>
        {props.children} 
      </Main>
    </Box>);
};

export default Layout




