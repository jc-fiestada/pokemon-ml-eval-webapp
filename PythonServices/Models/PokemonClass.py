from pydantic import BaseModel

class UserRequest(BaseModel):
    Quantity: int
    RandomState: int


