import React, { Component } from "react";
import { Route } from "react-router";
import { Layout } from "./Layout";
import Home from "./Home";
import AppErrorLogViewer from "./AppErrorLogViewer";

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path="/" component={Home} />
        <Route path="/error-log" component={AppErrorLogViewer} />
      </Layout>
    );
  }
}
