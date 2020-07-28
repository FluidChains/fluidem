import React from "react";
import { Route, Switch } from "react-router";
import ExceptionsList from "./ExceptionsList";
import ExceptionDetail from "./ExceptionDetail";
import { useLocation } from "react-router-dom";

export const ErrorLogViewer = () => {
  const location = useLocation();
  console.log(location);
  return (
    <div>
      <Switch>
        <Route path="**/exception-detail/:id" component={ExceptionDetail} />
        <Route exact path="*" component={ExceptionsList} />
      </Switch>
    </div>
  );
};
