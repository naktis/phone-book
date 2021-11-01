import '../shared.css';

function ServerError(props) {
  return(
    <span className="server-error">{props.children}</span>
  )
}

export default ServerError;