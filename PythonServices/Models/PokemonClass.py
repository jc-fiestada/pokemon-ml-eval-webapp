from pydantic import BaseModel

class UserRequest(BaseModel):
    quantity: int
    randomState: int


